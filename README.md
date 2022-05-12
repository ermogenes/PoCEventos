# PoCEventos
Uma prova de conceitos para um sistema web com event streaming

Subir o ambiente todo:
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
- Usar o `ksqldb-cli` para criar os streams contidos em [`data\ksqldb\PoCEventos.ksql`](data/ksqldb/PoCEventos.ksql):
```bash
docker-compose exec ksqldb-cli ksql http://ksqldb:8088
```

- Carregar o template do nifi contido em [`pipeline\nifi\PoCEventos.xml`](pipeline/nifi/PoCEventos.xml).

# Desenvolvimento

Subir um MySQL para desenvolvimento:
```
docker run --name mysql-dev -p 3307:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8.0.28
```

# TODO
- [ ] Adicionar migrations no ksqldb
- [ ] Configurar persistência do nifi
- [ ] Automatizar a criação dos templates e dos parâmetros do nifi na inicialização (ver `nifi import-param-context`)
- [ ] Conectar o nifi com o nifi-registry
- [ ] Usar mysql ou git para persistência do nifi-registry
- [ ] Adicionar testes de infra com [FluentDocker](https://github.com/mariotoffia/FluentDocker)
