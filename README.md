# Estudos Microsserviços

### Projeto para estudos de Microsserviços

Criação de Microsserviços, seguindo padrões e boas práticas. </br>
Padrão feito a partir de vários tutoriais e estruturas que achei interessante. </br>
Baseado no curso '.NET Core Microservices - The Complete Guide (.NET 8 MVC)', disponibilizado na Udemy por Bhrugen Patel:
</br>
https://www.udemy.com/course/net-core-microservices-the-complete-guide-net-6-mvc/


Vídeo do curso na versão de graça:
</br>
https://www.youtube.com/watch?v=Nw4AZs1kLAs
</br>

Obs.: cada projeto de serviço seguiu uma estrutura simples, pois o objetivo é o estudo do padrão de microsserviços.
</br>
</br>

---
### Histórico branches (em implementação):

Arquitetura utilizada para este projeto:
<img src="udemy-microservice.png" alt="imagem arquitetura microsserviços"></img>
- Linhas contínuas são comunicações síncronas
  - Exemplo: o ShoppingCartAPI precisa requisitar os detalhes dos Produos para o ProductsAPI, esperar o retorno, para depois enviar as informações para o frontend.
- Linhas tracejadas são comunicações assíncronas
  - Exemplo: quando requisitado o serviços de enviar email, no EmailAPI, não é esperado um retorno, apenas é solicitada a ação.

#### Anotações:
- Como ocorre a autenticação no MVC:
  1. Usuário faz o cadastro;
  2. Usuário faz login;
  3. No processo de login, o token que é retornado na resposta é armazenado em cookie (TokenProvider);
  4. Nas requisições que necessitam de autenticação, o token é adquirido pelo cookie (TokenProvider) e adicionado no header pelo BaseService (que é onde está implementado todas as requisições HTTP);

</br >

- Como ocorre a autenticação entre serviços (exemplo no ShoppingCartAPI):
  1. Após receber uma requisição com o token, vinda do projeto MVC, este token é armazenado pelo BackendApiAuthenticationHttpClientHandler;
  2. Toda requisição feita entre serviços é configurada para que este token seja adicionado.

</br >

- Não confundir o 'async' e 'await' que utilizamos em métodos, com os tipos de comunicações 'async' e 'sync'. Métodos com 'async' e 'await' servem para que a execução do código espere o retorno de algo que está como await, para que então continue. Comunicação 'sync' e 'async' é o que foi explicado abaixo da imagem (no desenho da arquitetura com as linhas contínuas e tracejadas).

</br >

<details>
<summary>Lógica para implementação do carrinho de compras (ShoppingCart):</summary>
<img src="udemy-microservice-shoppingcart-logic.png" alt="imagem arquitetura microsserviços"></img>
</details>

</br >

---
### Histórico branches (em implementação):

<details>
<summary>1 - CouponAPI</summary>
<ul>
  <li>Implementado estrutura base com projetos e pastas;</li>
  <li>Implementado API para Coupon;</li>
</ul> 
</details>

<details>
<summary>2 - AuthAPI</summary>
<ul>
    <li>Implementado API para Auth;</li>
    <li>Implementado autenticação com Token JWT.</li>
</ul> 
</details>

<details>
<summary>3 - ProductAPI</summary>
<ul>
    <li>Implementado API para Product.</li>
</ul> 
</details>

<details>
<summary>4 - Homepage e Details</summary>
<ul>
  <li>Implementado view da Homepage com cards de Products e a view de Details para detalhes do produto com botão de adicionar no carrinho.</li>
</ul> 
</details>

<details>
<summary>5 - Shopping Cart</summary>
<ul>
  <li>Implementado API para ShoppingCart.</li>
</ul> 
</details>

<details>
<summary>6 - Service Bus</summary>
<ul>
  <li></li>
</ul> 
</details>

<details>
<summary>7 - Email API</summary>
<ul>
  <li></li>
</ul> 
</details>

<details>
<summary>8 - Checkout e OrderAPI</summary>
<ul>
  <li></li>
</ul> 
</details>

<details>
<summary>9 - RewardsAPI</summary>
<ul>
  <li></li>
</ul> 
</details>

<details>
<summary>10 - Gateway</summary>
<ul>
  <li></li>
</ul> 
</details>

---

### Estudos (com links para referência):

<details>
<summary>O que é ASP.NET?</summary>
https://dotnet.microsoft.com/pt-br/learn/aspnet/what-is-aspnet
</details>

<details>
<summary>Vídeo: Microservice ASP.NET CORE Example</summary>
https://www.youtube.com/watch?v=6grbaE9fnUU
</details>

<details>
<summary>Vídeo: Redis + Microservices Architecture in 60 Seconds</summary>
https://www.youtube.com/watch?v=Su5l3XtimLw
</details>

<details>
<summary>Vídeo: Redis Crash Course - the What, Why and How to use Redis as your primary database</summary>
https://www.youtube.com/watch?v=OqCK95AS-YE
</details>

<details>
<summary>Vídeo: Why should you use Kafka in your microservice applications?</summary>
https://www.youtube.com/watch?v=cv5vqi5O9bY
</details>

<details>
<summary>ASP.NET Razor Pages vs MVC: How Do Razor Pages Fit in Your Toolbox?</summary>
https://stackify.com/asp-net-razor-pages-vs-mvc/
</details>

<details>
<summary>The API gateway pattern versus the Direct client-to-microservice communication</summary>
https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/direct-client-to-microservice-communication-versus-the-api-gateway-pattern
</details>

<details>
<summary>Microservices: REST vs Messaging</summary>
https://stackoverflow.com/questions/41010290/microservices-rest-vs-messaging
</details>

<details>
<summary>Rest API vs AMQP</summary>
https://stackoverflow.com/questions/59478191/rest-api-vs-amqp
</details>

<details>
<summary>Building a Microservices Ecosystem with Kafka Streams and KSQL</summary>
https://www.confluent.io/blog/building-a-microservices-ecosystem-with-kafka-streams-and-ksql/
</details>

<details>
<summary>Tutorial: Introduction to Streaming Application Development</summary>
https://docs.confluent.io/platform/current/tutorials/examples/microservices-orders/docs/index.html
</details>