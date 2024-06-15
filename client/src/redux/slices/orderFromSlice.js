import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { axiosInstance } from "../../config/config";
import { toastError, toastSuceess } from "../../util/reactToastify";
import axios from "axios";

export const fetchOrderFrom = createAsyncThunk(
  "orderFrom/fetchOrderFrom",
  async () => {
    //   const { data } = await axiosInstance.get("/api/BookApi");
    //   return data.data;
    //   try {
    //     const { data } = await axiosInstance.get(`/api/BookApi`);
    //     return data.data;
    //   } catch (error) {
    //     console.log(error.response.data.message);
    //     toastError(error?.response?.data?.message);
    //   }
    try {
      const response = await axios.get(
        "https://api.asianpublisher.in/api/BecomeAuthorApi"
      );
      // console.log("response", response);
      return response.data;
    } catch (error) {
      console.error("Error fetching books:", error);
    }
  }
);

export const fetchOrderFromDetails = createAsyncThunk(
  "orderFrom/fetchOrderFromDetails",
  async (id) => {
    try {
      const response = await axios.get(
        `https://api.asianpublisher.in/api/BecomeAuthorApi/${id}`
      );
      // console.log("response", response);
      return response.data;
    } catch (error) {
      toastError(error?.response?.data?.message);
      console.log(error.response.data.message);
    }
  }
);

export const createOrderFrom = createAsyncThunk(
  "orderFrom/createOrderFrom",
  async (orderFromrData) => {
    try {
      const response = await axios.post(
        "https://api.asianpublisher.in/api/OrderFormApi",
        orderFromrData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      if (
        response?.data?.message !== undefined &&
        response?.data?.message === "Success"
      ) {
        toastSuceess("Your Order has been submit successfully");
        // toastSuceess(response?.data?.message);
      }
      return response?.data;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

export const updateOrderFrom = createAsyncThunk(
  "orderFrom/updateOrderFrom",
  async (updateOrderFrom) => {
    try {
      const { data } = await axiosInstance.put(
        `/api/book/${updateOrderFrom.id}`,
        updateOrderFrom.orderFromData,
        {
          headers: {
            token: localStorage.getItem("token"),
          },
        }
      );
      if (data?.success) {
        toastSuceess(data?.message);
      }

      return data.data;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

export const deleteOrderFrom = createAsyncThunk(
  "orderFrom/deleteOrderFrom",
  async (id) => {
    try {
      const { data } = await axiosInstance.delete(`/api/book/${id}`, {
        headers: {
          token: localStorage.getItem("token"),
        },
      });
      if (data?.success) {
        toastSuceess(data?.message);
      }

      return id;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

const orderFromSlice = createSlice({
  name: "orderFrom",
  initialState: {
    orderFroms: [],
    loading: "idle",
    error: null,
    message: "",
    orderFromDetails: {},
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      //Get  all orderFrom
      .addCase(fetchOrderFrom.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchOrderFrom.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.orderFroms = action.payload;
      })
      .addCase(fetchOrderFrom.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Get orderFrom Details

      .addCase(fetchOrderFromDetails.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchOrderFromDetails.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.orderFromDetails = action.payload;
      })
      .addCase(fetchOrderFromDetails.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Create orderFrom

      .addCase(createOrderFrom.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(createOrderFrom.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.orderFroms.push(action?.payload?.data);
      })
      .addCase(createOrderFrom.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Update orderFrom

      .addCase(updateOrderFrom.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(updateOrderFrom.fulfilled, (state, action) => {
        const index = state.orderFroms.findIndex(
          (orderFrom) => orderFrom.id === action?.payload?.id
        );
        if (index !== -1) {
          state.orderFroms[index] = action.payload;
        }
      })
      .addCase(updateOrderFrom.rejected, (state) => {
        state.loading = "rejected";
      })

      //Delete orderFrom
      .addCase(deleteOrderFrom.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(deleteOrderFrom.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.orderFroms = state.orderFroms.filter(
          (orderFrom) => orderFrom?.id !== action.payload
        );
      })
      .addCase(deleteOrderFrom.rejected, (state) => {
        state.loading = "rejected";
      });
  },
});

export default orderFromSlice.reducer;
