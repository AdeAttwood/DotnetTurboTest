// Please see documentation at
// https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web
// assets.

// Write your JavaScript code.

const { HubConnectionBuilder } = window.signalR;
const { connectStreamSource, disconnectStreamSource } = window.Turbo;

class TurboStream extends EventTarget {
  #connection = null;

  constructor() {
    super();

    this.#connection = new HubConnectionBuilder().withUrl("/turbohub").build();
  }

  async connect() {
    if (this.#connection.state === "Connected") {
      return;
    }

    await this.#connection.start();
    this.#connection.on("TurboStream", this.#dispatchEvent.bind(this));
    connectStreamSource(this);
  }

  async disconnect() {
    if (this.#connection.state !== "Connected") {
      return;
    }

    await this.#connection.stop();
    disconnectStreamSource(this);
  }

  #dispatchEvent(data) {
    const event = new MessageEvent("message", { data });
    return stream.dispatchEvent(event);
  }
}

const stream = new TurboStream();
stream.connect();
