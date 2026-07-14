FROM node:20-alpine AS build
WORKDIR /app

COPY src/ToDo.Angular.UI/package*.json ./
RUN npm install -g ci
RUN npm ci

COPY ./src/ToDo.Angular.UI/ ./
RUN npm run build -- --configuration production

FROM nginx:1.27-alpine AS runtime

COPY --from=build /app/dist/todo-client /usr/share/nginx/html
COPY src/ToDo.Angular.UI/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
