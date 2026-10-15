## Onde a IA foi ultilizada no desenvolvimento do projeto?

Neste projeto a ia foi ultilizada para diversas coisas, dentre elas:
- Depuração de Código
- Ajuda na consulta da documentação oficial do dotnet
- Ajuda no desenvolvimento dos efeitos de Neumorphism
- Ajuda na implementação do mecanismo de busca trazendo importantes insights para manter o código dentro das regras descritas no ponto 02 do guia
- Ajuda no entendimento da biblioteca xunit para a criação dos testes Unitários
- Ajuda na documentação em especial na escrita do README.md

### Principais pontos de falha e atenção durante o uso da IA

Durande o decorrer do desenvolvimenteo do projeto tive uma série de falhas onde a IA alucinava ou tinha dificuldades que se não analizadas com atenção
poderiam acarretar em problemas ao decorrer do desenvolvimento, dentre eles:
- Constante quebra de arquitetura, por diversas vezes tomando praticas que iriam contra a arquitetura pre estabelecida o que em projetos maiores poderia custar caro na manutenibilidade do projeto
- Insistencia no uso de "Jeitinhos" ou "gambiarras" tecnicas perigosas quando o próprio dotnet tinha soluções próprias na documentação nativa (insistencia ocorria principalmente durante a depuração de desserialização do arquivo json)