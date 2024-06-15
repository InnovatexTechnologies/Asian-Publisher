import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.js";
import store from "./redux/store/index.js";
import { Provider } from "react-redux";
import { PersistGate } from "redux-persist/integration/react";
import { persistStore } from "redux-persist";

// import "bootstrap/dist/css/bootstrap.min.css";
// import "bootstrap/dist/js/bootstrap.bundle.min";

//Toast Container
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

//persisitor to persist state
let persistor = persistStore(store);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <App />
        <ToastContainer />
      </PersistGate>
    </Provider>
  </React.StrictMode>
);
