import { createSlice } from "@reduxjs/toolkit";
import { toastError, toastSuceess } from "../../util/reactToastify";

export const cartSlice = createSlice({
  name: "cart",
  initialState: {
    cartdata: [],
    quantity: 0,
    lastUpdated: null,
  },
  reducers: {
    addTocart: (state, action) => {
      const sevenDaysInMillis = 7 * 24 * 60 * 60 * 1000; // 7 days in milliseconds
      const checkcartdata = JSON.parse(JSON.stringify(state.cartdata));
      let {
        id,
        authors,
        bookCode,
        courseSemesters,
        image,
        mRP,
        name,
        ...rest
      } = {
        ...action.payload.product,
      };

      // const isItemExist = checkcartdata.find(
      //     (i) => i?.id === action?.payload.product.id
      // )
      // console.log(" action.payload.mRP", mRP, id);

      const isItemExist = findObjectFromArray(
        action.payload.product,
        checkcartdata
      );
      // console.log(" action.isItemExist", isItemExist);
      if (isItemExist) {
        toastError("Item already exists");
      }

      if (isItemExist) {
        const existingdata = checkcartdata.map((s) =>
          s === isItemExist ? action.payload.product : s
        );
        state.cartdata = existingdata;
        state.lastUpdated = Date.now() + sevenDaysInMillis;
      } else {
        state.cartdata.push(action.payload.product);
        state.quantity += 1;
        toastSuceess("Item add successfully");
        state.lastUpdated = Date.now() + sevenDaysInMillis;
      }
    },
    updateTocart: (state, action) => {
      const sevenDaysInMillis = 7 * 24 * 60 * 60 * 1000; // 7 days in milliseconds
      const checkcartdata = JSON.parse(JSON.stringify(state.cartdata));
      let { id, quantity, ...rest } = {
        ...action.payload.product,
      };

      const isItemExist = findObjectFromArray(
        action.payload.product,
        checkcartdata
      );
      // console.log(" action.isItemExist", isItemExist);

      if (isItemExist) {
        const sevenDaysInMillis = 7 * 24 * 60 * 60 * 1000;
        const indexToRemove = state.cartdata.findIndex(
          (item) => item.id === id
        );
        if (indexToRemove !== -1) {
          state.cartdata.splice(indexToRemove, 1, action.payload.product);
          toastSuceess("Item updated successfully");
          state.lastUpdated = Date.now() + sevenDaysInMillis;
        }
      } else {
        state.cartdata.push(action.payload.product);
        state.quantity += 1;
        toastSuceess("Item add successfully");
        state.lastUpdated = Date.now() + sevenDaysInMillis;
      }
    },

    removeTocart: (state, action) => {
      const sevenDaysInMillis = 7 * 24 * 60 * 60 * 1000;
      const removeitem = JSON.parse(JSON.stringify(state.cartdata));
      // console.log(" action.payload.item", action.payload);
      // console.log("removeitem removeitem", removeitem);

      const filtereddata = removeitem.filter(
        (book) => book.id !== action.payload
      );
      console.log("filtereddata filtereddata", filtereddata);
      state.cartdata = filtereddata;
      if (filtereddata.length < removeitem.length) {
        state.quantity -= 1;
      }
      toastSuceess("Item removed successfully");
      state.lastUpdated = Date.now() + sevenDaysInMillis;
    },

    resetCart: (state, action) => {
      state.cartdata = [];
      state.quantity = 0;
    },

    verifyCart: (state, action) => {
      let currentdate = Date.now();
      if (state.lastUpdated - currentdate < 0) {
        state.cartdata = [];
        state.quantity = 0;
      }
    },
    changePaymentMode: (state, action) => {
      const { index, item, paymentModeType } = action.payload;
      const cardData = JSON.parse(JSON.stringify(state.cartdata));
      // state.cartdata = [];
      // state.quantity = 0;

      if (paymentModeType === "Installment") {
        const { Installment, quantity } = cardData[index];

        let InstallmentPrice = Installment[0]?.Amount ?? 0;

        const myObj = {
          ...cardData[index],
          sellingType: paymentModeType,
          price: InstallmentPrice * quantity,
        };
        cardData.splice(index, 1, myObj);

        state.cartdata = cardData;
      }
      if (paymentModeType === "Normal") {
        const { OriginalPrice, quantity } = cardData[index];
        const myObj = {
          ...cardData[index],
          sellingType: paymentModeType,
          price: OriginalPrice * quantity,
        };
        cardData.splice(index, 1, myObj);
        state.cartdata = cardData;
      }
    },
  },
});

//Function to check from array of objects
function findObjectFromArray(searchObj, arr) {
  if (arr.length == 0) return false;
  let isMatch = false;
  for (let i = 0; i < arr.length; i++) {
    const currentObj = arr[i];
    if (Object.values(searchObj)[0] === Object.values(currentObj)[0]) {
      isMatch = true;
    }
  }
  return isMatch;
}

export const {
  addTocart,
  updateTocart,
  removeTocart,
  resetCart,
  verifyCart,
  changePaymentMode,
} = cartSlice.actions;

export default cartSlice.reducer;
