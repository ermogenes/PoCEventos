FROM confluentinc/cp-kafka-connect:6.2.4

# ---- Installing connectors from Confluent Hub (/usr/share/confluent-hub-components)
RUN confluent-hub install --no-prompt castorm/kafka-connect-http:0.8.6
RUN confluent-hub install --no-prompt dariobalinzo/kafka-connect-elasticsearch-source:1.3
RUN confluent-hub install --no-prompt confluentinc/kafka-connect-elasticsearch:11.1.0
RUN confluent-hub install --no-prompt mongodb/kafka-connect-mongodb:1.6.1
RUN confluent-hub install --no-prompt jcustenborder/kafka-connect-transform-common:0.1.0.54
RUN confluent-hub install --no-prompt mdrogalis/voluble:0.3.1
RUN confluent-hub install --no-prompt confluentinc/kafka-connect-jdbc:10.2.1

# ---- Installing JDBC drivers (/usr/share/java/jdbc ~> /usr/share/confluent-hub-components/confluentinc-kafka-connect-jdbc/lib)
RUN mkdir -p /usr/share/java/jdbc/mysql && curl -LsS https://dev.mysql.com/get/Downloads/Connector-J/mysql-connector-java-8.0.29.tar.gz | tar xvz -C /usr/share/java/jdbc/mysql --wildcards '*.jar'
RUN mkdir -p /usr/share/java/jdbc/mssql && curl -LsS https://go.microsoft.com/fwlink/?linkid=2168494 | tar xvz -C /usr/share/java/jdbc/mssql --wildcards '*.jar'
RUN cp -l $(du -a /usr/share/java/jdbc/ | awk '{print $2}' | grep .jar) /usr/share/confluent-hub-components/confluentinc-kafka-connect-jdbc/lib

## Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.9.0/wait /wait
USER root
RUN chmod +x /wait
USER appuser

CMD /wait && /etc/confluent/docker/run & sleep infinity