# Trade-offs de Características de Qualidade

As categorias de requisitos não-funcionais para o produto de software **PetGo**, conforme o modelo FURPS+, seriam:

1. **Usabilidade**:  
   a. A interface deve ser simples, clara e intuitiva, permitindo que pelo menos 90% dos usuários consigam realizar uma ação principal (ex.: marcar um passeio ou visualizar animais disponíveis para adoção) sem ajuda.  
   b. O design deve ser responsivo, adaptando-se a diferentes dispositivos (desktop, tablets e smartphones).  

2. **Desempenho**:  
   a. O sistema deve responder em até 5 segundos para as principais consultas, mesmo em períodos de pico de acesso.  
   b. Deve garantir fluidez na navegação e boa experiência de uso, mesmo com múltiplos usuários conectados simultaneamente.  

3. **Confiabilidade / Segurança**:  
   a. O sistema deve oferecer autenticação básica por login e senha, armazenadas de forma segura com técnicas de hash.  
   b. O histórico de interações deve ser registrado (ex.: simulações de processos de adoção) para garantir rastreabilidade e transparência.  
   c. Deve assegurar alta disponibilidade, minimizando falhas e interrupções do serviço.  

4. **Suportabilidade / Manutenibilidade**:  
   a. O sistema deve ser modular o suficiente para permitir ajustes de código de forma ágil durante o semestre.  
   b. A arquitetura deve permitir evolução contínua do software, incorporando novas funcionalidades a partir do feedback dos usuários.  

---

## Importância relativa de cada categoria

| Categoria       | Usabilidade | Desempenho | Confiabilidade | Suportabilidade |
|-----------------|-------------|------------|----------------|-----------------|
| Usabilidade     | -           | >          | >              | >               |
| Desempenho      | <           | -          | <              | >               |
| Confiabilidade  | <           | >          | -              | >               |
| Suportabilidade | <           | <          | <              | -               |

---

[Retorna](../README.md)
