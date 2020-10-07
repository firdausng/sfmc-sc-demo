# Setup

You need to install [Nodejs](https://nodejs.org/en/) to your system
Make sure you have at least version **12.18.4 LTS**


# Installation
Download all dependencies using below command

```sh
npm install
```

# Build
Use below command to build the project

```sh
npm run start
```

# Tunnel localhost
You need to download [ngrok](https://ngrok.com/) to tunnel your proejct from localhost to the internet

after download, go to download folder and run below command

Use below command to build the project
Change port to the port assigned by iis
```sh
ngrok http -host-header="localhost:[port]" [port]
```
