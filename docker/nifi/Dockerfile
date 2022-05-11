FROM apache/nifi:1.16.1

# ---- Installing JDBC drivers (/usr/share/java/jdbc)
USER root
RUN mkdir -p /usr/share/java/jdbc/mysql && curl -LsS https://dev.mysql.com/get/Downloads/Connector-J/mysql-connector-java-8.0.29.tar.gz | tar xvz -C /usr/share/java/jdbc/mysql --wildcards '*.jar'
RUN mkdir -p /usr/share/java/jdbc/mssql && curl -LsS https://go.microsoft.com/fwlink/?linkid=2168494 | tar xvz -C /usr/share/java/jdbc/mssql --wildcards '*.jar'
USER nifi

CMD sleep infinity