FROM node:12.19.0-alpine as front
WORKDIR /web
COPY ./Frontend/AngularProject/package.json ./package.json
COPY ./Frontend/AngularProject/package-lock.json ./package-lock.json
RUN export NODE_OPTIONS="--max-old-space-size=2048" && npx npm-force-resolutions && npm install 

ARG configuration=production
COPY ./Frontend/AngularProject/ /web
RUN export NODE_OPTIONS="--max-old-space-size=2048" && npm run build -- --configuration=$configuration

FROM nginx:1.16
COPY --from=front /web/dist/ /usr/share/nginx/html
COPY --from=front /web/nginx.conf /etc/nginx/conf.d/default.conf