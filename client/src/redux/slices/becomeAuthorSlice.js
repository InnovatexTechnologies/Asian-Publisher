import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { axiosInstance } from "../../config/config";
import { toastError, toastSuceess } from "../../util/reactToastify";
import axios from "axios";

export const fetchBecomeAuthors = createAsyncThunk(
  "becomeAuthor/fetchBecomeAuthors",
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

export const fetchBecomeAuthorDetails = createAsyncThunk(
  "becomeAuthor/fetchBecomeAuthorDetails",
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

export const createBecomeAuthor = createAsyncThunk(
  "becomeAuthor/createBecomeAuthor",
  async (becomeAuthorData) => {
    try {
      const response = await axios.post(
        "https://api.asianpublisher.in/api/BecomeAuthorApi",
        becomeAuthorData,
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
        toastSuceess("Your request has been submit successfully");
        // toastSuceess(response?.data?.message);
      }
      return response?.data;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

export const updateBecomeAuthor = createAsyncThunk(
  "becomeAuthor/updateBecomeAuthor",
  async (updateBecomeAuthor) => {
    try {
      const { data } = await axiosInstance.put(
        `/api/book/${updateBecomeAuthor.id}`,
        updateBecomeAuthor.becomeAuthorData,
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

export const deleteBecomeAuthor = createAsyncThunk(
  "becomeAuthor/deleteBecomeAuthor",
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

const becomeAuthorSlice = createSlice({
  name: "becomeAuthor",
  initialState: {
    becomeAuthors: [],
    loading: "idle",
    error: null,
    message: "",
    becomeAuthorDetails: {},
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      //Get  all becomeAuthors
      .addCase(fetchBecomeAuthors.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchBecomeAuthors.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.becomeAuthors = action.payload;
      })
      .addCase(fetchBecomeAuthors.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Get becomeAuthors Details

      .addCase(fetchBecomeAuthorDetails.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchBecomeAuthorDetails.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.becomeAuthorDetails = action.payload;
      })
      .addCase(fetchBecomeAuthorDetails.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Create becomeAuthors

      .addCase(createBecomeAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(createBecomeAuthor.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.becomeAuthors.push(action?.payload?.data);
      })
      .addCase(createBecomeAuthor.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Update becomeAuthors

      .addCase(updateBecomeAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(updateBecomeAuthor.fulfilled, (state, action) => {
        const index = state.becomeAuthors.findIndex(
          (becomeAuthor) => becomeAuthor.id === action?.payload?.id
        );
        if (index !== -1) {
          state.becomeAuthors[index] = action.payload;
        }
      })
      .addCase(updateBecomeAuthor.rejected, (state) => {
        state.loading = "rejected";
      })

      //Delete becomeAuthors
      .addCase(deleteBecomeAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(deleteBecomeAuthor.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.becomeAuthors = state.becomeAuthors.filter(
          (becomeAuthor) => becomeAuthor?.id !== action.payload
        );
      })
      .addCase(deleteBecomeAuthor.rejected, (state) => {
        state.loading = "rejected";
      });
  },
});

export default becomeAuthorSlice.reducer;
