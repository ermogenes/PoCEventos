# PoCEventos
![Apache Kafka](https://img.shields.io/badge/Apache%20Kafka-000?style=for-the-badge&logo=apachekafka)
![ksqlDB](https://img.shields.io/badge/ksqlDB-000?style=for-the-badge&logo=apacherocketmq)
![Apache Nifi](https://img.shields.io/badge/Apache%20Nifi-%23D42029.svg?style=for-the-badge&logo=drupal&logoColor=white)
![Apache Groovy](https://img.shields.io/badge/Apache%20Groovy-4298B8.svg?style=for-the-badge&logo=Apache+Groovy&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![MySQL](https://img.shields.io/badge/mysql-%2300f.svg?style=for-the-badge&logo=mysql&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

Uma prova de conceitos para um sistema web com event streaming.

```
docker-compose up
```

Aplicação em http://localhost:8888/.

![](art/web-screenshot.png)

Nifi em http://localhost:48443/nifi/.

![](art/nifi-poceventos.png)

MySQL na porta 3308, usuário `root` e senha `root`.

![](art/mysql-loja-erd.png)

# Passos manuais
- Usar o `ksqldb-cli` para criar os streams contidos em [`data/ksqldb/PoCEventos.ksql`](data/ksqldb/PoCEventos.ksql):
```bash
docker exec -it poceventos_ksqldb-cli ksql http://ksqldb:8088
```

- Carregar o template do nifi contido em [`pipeline/nifi/PoCEventos.xml`](pipeline/nifi/PoCEventos.xml).

# Desenvolvimento

Subir um MySQL para desenvolvimento:
```
docker run --name mysql-dev -p 3307:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8.0.29
```

# TODO
- [ ] Adicionar migrations no ksqldb
- [ ] Configurar persistência do nifi
- [ ] Automatizar a criação dos templates e dos parâmetros do nifi na inicialização (ver `nifi import-param-context`)
- [ ] Conectar o nifi com o nifi-registry
- [ ] Usar mysql ou git para persistência do nifi-registry
- [ ] Adicionar testes de infra com [FluentDocker](https://github.com/mariotoffia/FluentDocker)
