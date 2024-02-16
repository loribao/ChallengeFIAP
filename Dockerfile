FROM rust:1.75.0-alpine as build

WORKDIR /usr/src/app
COPY . .
RUN cargo build --release --target-dir /usr/src/dist

FROM nginx:alpine as production
COPY --from=build /usr/src/dist /usr/share/nginx/html
asdfa
