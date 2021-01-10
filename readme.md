
# all services
`docker-compose up -d --build`

# postgres
`docker run -v /home/postgres:/var/lib/postgresql/data -d --restart=always -p 5432:5432 -e POSTGRES_PASSWORD=Qwerty123321 --name postgres postgres:12`


