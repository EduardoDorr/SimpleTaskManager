FROM node:22.21-alpine AS build

WORKDIR /app
ARG VITE_API_BASE_URL
COPY package*.json ./

RUN npm ci

COPY . .
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
RUN npm run build

FROM nginx:stable-alpine-slim

RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]