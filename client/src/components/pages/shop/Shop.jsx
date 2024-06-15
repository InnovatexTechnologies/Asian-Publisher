import React, { useState, useEffect, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import { fetchBooks } from "../../../redux/slices/bookSlice";
import { fetchAuthors } from "../../../redux/slices/authorSlice";
import { fetchCourses } from "../../../redux/slices/courseSlice";
import { fetchSemesters } from "../../../redux/slices/semesterSlice";
import { addTocart } from "../../../redux/slices/cartSlice";
import { REACT_APP_URL } from "../../../config/config";
import { FaCartPlus } from "react-icons/fa";
import Spinner from "../../common/Spinner";
import Checkbox from "@mui/material/Checkbox";
import ShopHeaderImage from "../../../Images/ShopHeaderImage.png";
import "../../../css/Style.css";
import "../../../css/bootstrap.min.css";
import { FaShoppingBag } from "react-icons/fa";
function Shop() {
  const { loading, books } = useSelector((state) => state.book);
  const { loading: coursesLoading, courses } = useSelector(
    (state) => state.course
  );
  const { loading: semestersLoading, semesters } = useSelector(
    (state) => state.semester
  );
  const { loading: authorsLoading, authors } = useSelector(
    (state) => state.author
  );
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const history = useLocation();
  const [loader, setLoader] = useState(true);
  const [allBooks, setAllBooks] = useState([]);
  const [checked, setChecked] = useState(false);
  const [checkedArr, setCheckedArr] = useState([]);
  const [allAuthors, setAllAuthors] = useState([]);
  const [allCourses, setAllCourses] = useState([]);
  const [allSemesters, setAllSemesters] = useState([]);
  const [filterAuthors, setFilterAuthors] = useState([]);
  const [filterCourses, setFilterCourses] = useState([]);
  const [filterSemesters, setFilterSemesters] = useState([]);
  const [orderFilter, setOrderFilter] = useState("");
  const [searchFilter, setSearchFilter] = useState("");
  const author = history.state;

  useMemo(() => {
    if (author && typeof author === typeof "String") {
      if (author !== undefined && author !== "") {
        setSearchFilter(author);
      }
    } else {
      if (author?.id !== undefined && author?.id !== "") {
        setFilterAuthors((prev) => [...prev, author?.id]);
        setCheckedArr((prev) => [...prev, author?.id]);
      }
    }
  }, [author]);

  useEffect(() => {
    dispatch(
      fetchBooks({
        filterAuthors,
        filterCourses,
        filterSemesters,
        orderFilter,
        searchFilter,
      })
    );
  }, [
    dispatch,
    filterSemesters,
    filterCourses,
    filterAuthors,
    orderFilter,
    checked,
    searchFilter,
  ]);
  useEffect(() => {
    dispatch(fetchAuthors());
  }, [dispatch]);
  useEffect(() => {
    dispatch(fetchCourses());
  }, [dispatch]);
  useEffect(() => {
    dispatch(fetchSemesters());
  }, [dispatch]);

  useEffect(() => {
    if (loading === "fulfilled") {
      setAllBooks(books);
      setAllAuthors(authors);
      setAllCourses(courses);
      setAllSemesters(semesters);
      setLoader(false);
    }
  }, [loading]);

  function handleCart(book) {
    const {
      id,
      authors,
      bookCode,
      courseSemesters,
      image,
      isFeatured,
      languageId,
      languageNav,
      mRP,
      numId,
      name,
      ...rest
    } = book;

    dispatch(
      addTocart({
        product: {
          id,
          quantity: 1,
          authors,
          bookCode,
          courseSemesters,
          image,
          isFeatured,
          languageId,
          languageNav,
          mRP,
          numId,
          name,
        },
      })
    );
  }

  function handleSort(e) {
    setOrderFilter(e.target.value);
    setLoader(true);
  }
  function coursesChangeHandler(e, newVal) {
    setLoader(true);
    setChecked(e.target.checked);
    if (e.target.checked) {
      if (newVal.type === "Course") {
        setFilterCourses((prev) => [...prev, newVal?.id]);
      }
    } else {
      setFilterCourses((prev) => prev.filter((id) => id !== newVal.id));
    }
  }

  function semestersChangeHandler(e, newVal) {
    setLoader(true);
    setChecked(e.target.checked);
    if (e.target.checked) {
      if (newVal.type === "Semester") {
        setFilterSemesters((prev) => [...prev, newVal?.id]);
      }
    } else {
      setFilterSemesters((prev) => prev.filter((id) => id !== newVal.id));
    }
  }

  function authorsChangeHandler(e, newVal) {
    setLoader(true);
    setChecked(e.target.checked);
    if (e.target.checked) {
      if (newVal.type === "Author") {
        setFilterAuthors((prev) => [...prev, newVal?.id]);
        setCheckedArr((prev) => [...prev, newVal?.id]);
      }
    } else {
      setFilterAuthors((prev) => prev.filter((id) => id !== newVal.id));
      setCheckedArr((prev) => prev.filter((id) => id !== newVal.id));
    }
  }

  return (
    <>
      {loader && <Spinner />}
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${ShopHeaderImage})`,
          "background-size": "cover",
          "background-position": "center",
        }}
      >
        <div className="gradient-overlay" />
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
        <div className="col-lg-8" style={{ float: "left" }}>
          <div
            className="video-content"
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "padding-top": "150px",
            }}
          >
            <p
              className="text"
              style={{
                "-webkit-text-transform": "uppercase",
                "text-transform": "uppercase",
                "font-size": "50px",
                "font-weight": "600",
                "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
                "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Shop
            </p>
            <p style={{ "font-size": "20px" }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
            </p>
          </div>
        </div>
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
      </div>
      <br />
      <div
        className="row"
        id="AboutUsSection"
        style={{
          display: "block",
          margin: "0px",
          padding: "0px",
          clear: "both",
        }}
      >
        <div className="col-lg-3" style={{ float: "left" }}>
          <div className="col-lg-12">
            <h4>Authors</h4>
            <hr />
            <div
              className="col-lg-12"
              style={{ height: "20vh", "overflow-y": "scroll" }}
            >
              {allAuthors?.length > 0 &&
                allAuthors.map((item, index) => (
                  <>
                    <div
                      key={index}
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        padding: "0 10px 0 10px",
                      }}
                    >
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                        }}
                      >
                        <div>
                          <Checkbox
                            checked={checkedArr?.includes(item.id)}
                            onChange={(e) => authorsChangeHandler(e, item)}
                          />
                        </div>
                        <div>{item?.name} </div>
                      </div>
                    </div>
                  </>
                ))}
            </div>
          </div>
          <br />
          <div className="col-lg-12">
            <h4>Courses</h4>
            <hr />
            <div
              className="col-lg-12"
              style={{ height: "20vh", "overflow-y": "scroll" }}
            >
              {allSemesters?.length > 0 &&
                allSemesters.map((item, index) => (
                  <>
                    <div
                      key={index}
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        padding: "0 10px 0 10px",
                      }}
                    >
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                        }}
                      >
                        <div>
                          <Checkbox
                            onChange={(e) => semestersChangeHandler(e, item)}
                          />
                        </div>
                        <div>{item?.name} </div>
                      </div>
                    </div>
                  </>
                ))}
            </div>
          </div>
          <br />
          <div className="col-lg-12">
            <h4>Semester</h4>
            <hr />
            <div
              className="col-lg-12"
              style={{ height: "20vh", "overflow-y": "scroll" }}
            >
              {allCourses?.length > 0 &&
                allCourses.map((item, index) => (
                  <>
                    <div
                      key={index}
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        padding: "0 10px 0 10px",
                      }}
                    >
                      <div
                        style={{
                          display: "flex",
                          alignItems: "center",
                        }}
                      >
                        <div>
                          <Checkbox
                            onChange={(e) => coursesChangeHandler(e, item)}
                          />
                        </div>
                        <div>{item?.name} </div>
                      </div>
                    </div>
                  </>
                ))}
            </div>
          </div>
        </div>
        <div className="col-lg-9" style={{ float: "left" }}>
          <div className="shop-top-bar">
            <div className="select-shoing-wrap">
              <div className="shop-select col-lg-4">
                <label htmlFor="SortBy">Sort by :</label>&nbsp;&nbsp;
                <select
                  onChange={handleSort}
                  name="SortBy"
                  id="SortBy"
                  className="form-control"
                >
                  <option value="manual">Sort By</option>
                  <option value="titleAscending">Alphabetically, A-Z</option>
                  <option value="titleDescending">Alphabetically, Z-A</option>
                  <option value="priceAscending">Price, low to high</option>
                  <option value="priceDescending">Price, high to low</option>
                </select>
              </div>
            </div>
          </div>
          <div className="row aboutcontent w-100">
            {allBooks &&
              allBooks.length > 0 &&
              allBooks.map((book) => (
                <div className="col-lg-4" style={{ float: "left" }}>
                  <center>
                    <div className="book">
                      <img
                        className="book-cover"
                        src={`${REACT_APP_URL}/Image/${book.image}`}
                        alt="Book Cover"
                        onClick={() => {
                          navigate(`/BookDetails/${book.id}`);
                        }}
                      />
                      <div className="book-inside" />
                    </div>
                  </center>
                  <p
                    style={{
                      "font-size": "15px",
                      "-webkit-text-align": "center",
                      "text-align": "center",
                      "margin-top": "15px",
                      "font-weight": "700",
                    }}
                  >
                    <span style={{ "font-size": "12px", "font-weight": "500" }}>
                      ISBN No. {book?.iSBN} &nbsp;&nbsp;
                      <button
                        onClick={() => handleCart(book)}
                        style={{
                          border: "none",
                          fontSize: "18px",
                          backgroundColor: "#fff",
                          color: "red",
                        }}
                      >
                        <FaCartPlus />
                        </button>
                        &nbsp;&nbsp;
                        <button
                        onClick={() => handleCart(book)}
                        style={{
                          border: "none",
                          fontSize: "18px",
                          backgroundColor: "#fff",
                          color: "red",
                        }}
                      >
                        <FaShoppingBag style={{color:"#000"}}
                          onClick={() => {
                            navigate("/checkout");
                          }}
                        />
                        </button>
                    </span>
                    <br />
                    {book?.name}
                    <br />
                    {book?.authors && book?.authors.length > 0 && (
                      <a href="#" className="remove_href">
                        {book?.authors.map((author, index) => (
                          <span
                            key={author.id}
                            style={{
                              "font-size": "12px",
                              "font-weight": "500",
                            }}
                          >
                            {author.name}
                            {index !== authors.length - 1 ? ", " : ""}
                          </span>
                        ))}
                      </a>
                    )}
                    <br />
                    <span
                      style={{
                        color: "red",
                        "font-size": "16px",
                        "font-weight": "600",
                      }}
                    >
                      Rs. {book?.mRP}
                    </span>
                  </p>
                </div>
              ))}
          </div>
        </div>
      </div>
      <div
        className="row"
        style={{
          clear: "both",
          height: "5vh",
          background: "linear-gradient(to top, rgb(216, 32, 40, 0.1), #fff)",
        }}
      />
      <Footer />
    </>
  );
}

export default Shop;
