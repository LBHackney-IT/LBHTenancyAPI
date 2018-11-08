FROM microsoft/mssql-server-linux

## Default SQL Server TCP/Port.
EXPOSE 1433

COPY ./ /usr/src/app
WORKDIR /usr/src/app
ENV SA_PASSWORD "Rooty-Tooty"
ENV ACCEPT_EULA "Y"

RUN /opt/mssql/bin/sqlservr &\
echo 'waiting for sever to start before running migrations\n' &&\
while ! /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Rooty-Tooty -d master  -Q "SELECT 1;"; do \
    echo 'Waiting for db'; sleep 3; \
done &&\
echo '--- running migrationns ---\n' &&\ 
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Rooty-Tooty -d master -i /usr/src/app/StubUniversalHousing/setup.sql
