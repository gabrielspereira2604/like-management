# Like Management API

API para gerenciar curtidas de posts, construída com .NET 9 e SQLite.

## Contexto

Projeto criado para demonstrar como implementar um contador de curtidas de forma correta em cenários de alta concorrência, evitando race conditions (lost update).

## O problema

A forma ingênua de incrementar um contador gera race condition:

```csharp
// ERRADO
var post = await db.Posts.FindAsync(postId); // lê: 10
post.LikeCount++;                             // calcula no back-end: 11
await db.SaveChangesAsync();                  // salva: 11
```

Duas requisições simultâneas lendo o valor `10` vão ambas salvar `11`, perdendo uma curtida.

## A solução

Delegar o incremento ao banco de dados, que executa a operação atomicamente:

```csharp
// CORRETO
await db.Posts
    .Where(p => p.Id == postId)
    .ExecuteUpdateAsync(s => s.SetProperty(p => p.LikeCount, p => p.LikeCount + 1));
```

SQL gerado:

```sql
UPDATE Posts SET LikeCount = LikeCount + 1 WHERE Id = @postId
```

O banco faz o lock, lê e incrementa em uma única instrução. Requisições concorrentes serializam no banco e o resultado sempre é consistente.

## Arquitetura

```
Controllers/
└── LikeController.cs     recebe as requisições HTTP

Services/
├── ILikeService.cs
└── LikeService.cs        regra de negócio

Repositories/
├── IPostRepository.cs
└── PostRepository.cs     acesso ao banco, incremento atômico

Models/
└── Post.cs               Id, Title, LikeCount

Data/
└── AppDbContext.cs
```

O padrão controller → service → repository desacopla a regra de negócio da persistência, facilitando testes unitários via mock da interface do repositório.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/posts` | Lista todos os posts |
| GET | `/api/posts/{id}` | Busca post por ID |
| POST | `/api/posts` | Cria um post |
| POST | `/api/posts/{id}/like` | Curte um post |
| DELETE | `/api/posts/{id}/like` | Remove curtida |

## Como rodar

```bash
cd LikeManagement.API
dotnet run
```

O banco SQLite (`likes.db`) é criado automaticamente na primeira execução.

## Trade-offs

O modelo atual guarda apenas o contador por post, sem registrar qual usuário curtiu. Isso significa:

- Leitura do contador é O(1)
- Não há como auditar quem curtiu
- Um usuário pode curtir várias vezes

Para impedir curtidas duplicadas seria necessário uma tabela `Likes (PostId, UserId)` com unique constraint, mantendo o `LikeCount` como cache desnormalizado para leitura rápida.
