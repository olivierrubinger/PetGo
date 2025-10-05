# Especificação de APIs

> A especificação de APIs descreve os principais endpoints da API RESTful do produto
> de software, os métodos HTTP associados a cada endpoint, suas descrições, os formatos
> de respostas, os parâmetros de URL esperados e o mecanismo de autenticação e autorização 
> utilizado.

| Endpoint                             | Método | Descrição                                      | Parâmetros                        | Formato da Resposta | Autenticação e Autorização |
|--------------------------------------|--------|------------------------------------------------|-----------------------------------|---------------------|----------------------------|
| /api/users/{user_id}/tasks/          | POST    | Agendar um novo passeio.                      | Corpo: tutor_id, pet_id, data_hora, duracao (minutos), endereco.                  | JSON(detalhes do passeio criado)              | JWT Token (Apenas Tutor ou Sistema)                  |
|/api/passeios | GET   | Listar todos os passeios.                          | Query: status, data_inicio, tipo_usuario (Tutor/Passeador). | JSON (Lista de passeios)              | JWT Token (Todos os usuários autenticados)              |
| /api/passeios/{passeio_id}| GET    | Obter detalhes de um passeio específico.       | URL: passeio_id (string/UUID). | JSON (Detalhes do passeio)                | JWT Token (Tutor ou Passeador envolvido)            |
| /api/passeios/{passeio_id} | PUT    | Atualizar um passeio (ex: mudar hora/data). | URL: passeio_id (string/UUID). Corpo: Campos a serem alterados. | JSON (Detalhes do passeio atualizado)               | JWT Token (Apenas Tutor que criou)             |
| /api/passeios/{passeio_id}/cancelar | POST | Cancelar um passeio.                 | URL: passeio_id (string/UUID). Corpo: motivo_cancelamento. | JSON (status: cancelado)                | JWT Token (Tutor ou Passeador envolvido)      |

[Retorna](../README.md)