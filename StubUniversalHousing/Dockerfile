FROM microsoft/mssql-server-linux

## Default SQL Server TCP/Port.
EXPOSE 1433

COPY ./ /usr/src/app
WORKDIR /usr/src/app
ENV SA_PASSWORD "Rooty-Tooty"
ENV ACCEPT_EULA "Y"

# Run SQL Server process and create tables.
RUN /opt/mssql/bin/sqlservr & sleep 20 \
  && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Rooty-Tooty -d master -i /usr/src/app/StubUniversalHousing/setup.sql
