import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { axiosInstance } from "../../config/config";
import { toastError, toastSuceess } from "../../util/reactToastify";
import axios from "axios";

export const fetchAuthors = createAsyncThunk(
  "author/fetchAuthors",
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
        "https://api.asianpublisher.in/api/AuthorApi"
      );
      //   console.log("response", response);
      return response.data;
    } catch (error) {
      console.error("Error fetching books:", error);
    }
  }
);
export const countBooksByAuthor = createAsyncThunk(
  "author/countBooksByAuthor",
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
        "https://api.asianpublisher.in/api/BookAuthorApi"
      );
      //   console.log("response", response);
      return response.data;
    } catch (error) {
      console.error("Error fetching books:", error);
    }
  }
);

export const fetchAuthorDetails = createAsyncThunk(
  "author/fetchAuthorDetails",
  async (id) => {
    try {
      const { data } = await axiosInstance.get(`/api/author/${id}`);

      return data.data;
    } catch (error) {
      toastError(error?.response?.data?.message);
      console.log(error.response.data.message);
    }
  }
);

export const createAuthor = createAsyncThunk(
  "author/createAuthor",
  async (bookData) => {
    try {
      const { data } = await axiosInstance.post("/api/author", bookData, {
        headers: {
          token: localStorage.getItem("token"),
        },
      });
      if (data?.success) {
        toastSuceess(data?.message);
      }
      return data;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

export const updateAuthor = createAsyncThunk(
  "author/updateAuthor",
  async (updateAuthor) => {
    try {
      const { data } = await axiosInstance.put(
        `/api/author/${updateAuthor.id}`,
        updateAuthor.authorData,
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

export const deleteAuthor = createAsyncThunk(
  "author/deleteAuthor",
  async (authorId) => {
    try {
      const { data } = await axiosInstance.delete(`/api/author/${authorId}`, {
        headers: {
          token: localStorage.getItem("token"),
        },
      });
      if (data?.success) {
        toastSuceess(data?.message);
      }

      return authorId;
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
  }
);

const authorSlice = createSlice({
  name: "author",
  initialState: {
    authors: [],
    countBookauthor: [],
    loading: "idle",
    error: null,
    message: "",
    authorDetails: {},
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      //Get  all author
      .addCase(fetchAuthors.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchAuthors.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.authors = action.payload;
      })
      .addCase(fetchAuthors.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })
      //Get  all authors by book count
      .addCase(countBooksByAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(countBooksByAuthor.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.countBookauthor = action.payload;
      })
      .addCase(countBooksByAuthor.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Get author Details

      .addCase(fetchAuthorDetails.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(fetchAuthorDetails.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.authorDetails = action.payload;
      })
      .addCase(fetchAuthorDetails.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Create author

      .addCase(createAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(createAuthor.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.authors.push(action?.payload?.data);
      })
      .addCase(createAuthor.rejected, (state, action) => {
        state.loading = "rejected";
        state.error = action.error.message;
      })

      //Update author

      .addCase(updateAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(updateAuthor.fulfilled, (state, action) => {
        const index = state.authors.findIndex(
          (author) => author._id === action?.payload?._id
        );
        if (index !== -1) {
          state.authors[index] = action.payload;
        }
      })
      .addCase(updateAuthor.rejected, (state) => {
        state.loading = "rejected";
      })

      //Delete author
      .addCase(deleteAuthor.pending, (state) => {
        state.loading = "pending";
      })
      .addCase(deleteAuthor.fulfilled, (state, action) => {
        state.loading = "fulfilled";
        state.authors = state.authors.filter(
          (author) => author?._id !== action.payload
        );
      })
      .addCase(deleteAuthor.rejected, (state) => {
        state.loading = "rejected";
      });
  },
});

export default authorSlice.reducer;
