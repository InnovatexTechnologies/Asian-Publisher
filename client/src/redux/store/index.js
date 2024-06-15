import { configureStore } from "@reduxjs/toolkit";
import { combineReducers } from "redux";
import bookSlice from "../slices/bookSlice";
import authorSlice from "../slices/authorSlice";
import courseSlice from "../slices/courseSlice";
import cartSlice from "../slices/cartSlice";
import semesterSlice from "../slices/semesterSlice";
import becomeAuthorSlice from "../slices/becomeAuthorSlice";
import orderFromSlice from "../slices/orderFromSlice";
//persisted Reducer
import storage from "redux-persist/lib/storage";
import {
  persistReducer,
  FLUSH,
  REHYDRATE,
  PAUSE,
  PERSIST,
  PURGE,
  REGISTER,
} from "redux-persist";
import { encryptTransform } from "redux-persist-transform-encrypt";

const reducers = combineReducers({
  /* here we will be adding reducers*/
  book: bookSlice,
  cart: cartSlice,
  author: authorSlice,
  course: courseSlice,
  semester: semesterSlice,
  becomeAuthor: becomeAuthorSlice,
  orderFrom: orderFromSlice,
});

const persistConfig = {
  key: "root",
  storage,
  transforms: [
    encryptTransform({
      secretKey: "your-secret-key",
      //You Can Also remove by using this methods
      //   onError: function (error) {
      //     // Handle the error.
      //     // localStorage.clear()
      //     localStorage.removeItem("persist:root");
      //   },
    }),
  ],
  timeout: 1000,
  onRehydrate: () => {
    localStorage.removeItem("persist:root");
    // Handle the scenario where the data has been tampered with
    // You can check the integrity of the data here and decide what action to take
    // In this example, we will remove the data from local storage if it has been tampered with
    PURGE();
  },
  blacklist: [
    "book",
    "author",
    "semester",
    "course",
    "becomeAuthor",
    "orderFrom",
  ],

  //Blacklisting and white listing Store
  // whitelist:["tags"]
};

const persistedReducer = persistReducer(persistConfig, reducers);

export default configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }),
});

// import { configureStore } from "@reduxjs/toolkit";
// import { combineReducers } from "redux";
// import bookSlice from "../slices/bookSlice";
// import authorSlice from "../slices/authorSlice";
// import courseSlice from "../slices/courseSlice";
// import cartSlice from "../slices/cartSlice";
// import semesterSlice from "../slices/semesterSlice";

// const reducer = combineReducers({
//   /* here we will be adding reducers*/
//   book: bookSlice,
//   cart: cartSlice,
//   author: authorSlice,
//   course: courseSlice,
//   semester: semesterSlice,
// });

// /*Define the store using configureStore*/
// const store = configureStore({
//   reducer: reducer,
// });

// export default store;
