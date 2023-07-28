# Trade-offs de Características de Qualidade

As categorias de requisitos não-funcionais para o produto de software "FocusFlow", conforme o modelo FURPS+, seriam:

1. **Usabilidade**: O sistema deve ser fácil de usar, com uma interface intuitiva e amigável. Métrica: 90% dos usuários devem ser capazes de completar uma tarefa sem assistência após uma única sessão de treinamento.

2. **Desempenho**: O sistema deve ser capaz de responder rapidamente às ações do usuário, independentemente do número de tarefas gerenciadas. Métrica: O tempo de resposta para qualquer ação do usuário não deve exceder 2 segundos.

3. **Confiabilidade**: O sistema deve ser robusto e livre de erros, com mecanismos para lidar com falhas e garantir a recuperação de dados. Métrica: O sistema deve ter uma disponibilidade de 99,9% (menos de 8,76 horas de inatividade por ano).

4. **Suportabilidade**: O sistema deve ser projetado de forma a facilitar atualizações e manutenção futuras. Métrica: Todas as solicitações de suporte devem ser respondidas dentro de 24 horas.

Agora, vamos considerar a importância relativa de cada categoria:

| Categoria | Usabilidade | Desempenho | Confiabilidade | Suportabilidade |
| --- | --- | --- | --- | --- |
| Usabilidade | - | > | > | > |
| Desempenho | < | - | > | > |
| Confiabilidade | < | < | - | > |
| Suportabilidade | < | < | < | - |

Nesta matriz, o sinal ">" indica que a categoria da linha é mais importante que a categoria da coluna, e o sinal "<" indica que a categoria da linha é menos importante que a categoria da coluna. Por exemplo, a Usabilidade é considerada mais importante que o Desempenho, Confiabilidade e Suportabilidade, enquanto o Desempenho é considerado mais importante que a Confiabilidade e Suportabilidade, mas menos importante que a Usabilidade, e assim por diante.
