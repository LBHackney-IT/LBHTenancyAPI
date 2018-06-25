cd LBHTenancyAPI
heroku container:push web --context-path=.. --app=lbh-tenancy-api-$1
heroku container:release web --app=lbh-tenancy-api-$1
