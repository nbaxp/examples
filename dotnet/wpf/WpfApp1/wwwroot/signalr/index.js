import * as signalR from "@microsoft/signalr";
import PubSub from "pubsub-js";
import useAppStore from "../store/app.js";
import { isLogin } from "../api/user.js";

let connectionId = null;
const connection = new signalR.HubConnectionBuilder()
  .withUrl("./api/hub", {
    accessTokenFactory: () => {
      return useAppStore().token;
    },
  })
  //.withAutomaticReconnect()
  .build();
const connect = async () => {
  if (await isLogin()) {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
      connection
        .start()
        .then(function () {
          console.log("signalr connected");
        })
        .catch(async function (error) {
          console.log(error);
          await isLogin();
          setTimeout(connect, 5000);
        });
    }
  }
};
connection.onclose(async () => {
  await connect();
});
connection.on("connected", (id) => {
  connectionId = id;
});
connection.on("ServerToClient", (method, data) => {
  PubSub.publish(method, data);
});

export { connection, connect, connectionId };
