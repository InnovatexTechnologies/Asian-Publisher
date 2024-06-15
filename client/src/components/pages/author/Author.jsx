import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { countBooksByAuthor } from "../../../redux/slices/authorSlice";
import { REACT_APP_URL } from "../../../config/config";
import { useNavigate } from "react-router-dom";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import authorpageimage from "../../../Images/authorpageimage.png";
import Spinner from "../../common/Spinner";

function Author() {
  const { loading, countBookauthor } = useSelector((state) => state.author);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [countBookByAuthor, setCountBookByAuthor] = useState([]);
  const [loader, setLoader] = useState(true);
  useEffect(() => {
    dispatch(countBooksByAuthor());
  }, [dispatch]);
  useEffect(() => {
    if (loading === "fulfilled") {
      setCountBookByAuthor(countBookauthor);
      setLoader(false);
    }
  }, [loading]);

  function handleAuthor(author) {
    navigate("/shop", {
      state: { id: author?.authorNav?.id, name: author?.authorNav?.name },
    });
  }
  return (
    <>
      {loader && <Spinner />}
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${authorpageimage})`,

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
              Our Authors
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
        <div className="container-fluid">
          {countBookByAuthor &&
            countBookByAuthor.length > 0 &&
            countBookByAuthor.map((author) => {
              if (author?.authorNav?.content !== "") {
                return (
                  <div
                    key={author.id}
                    className="col-lg-6"
                    style={{
                      float: "left",
                      "margin-bottom": "60px",
                    }}
                  >
                    <div className="col-lg-1" style={{ float: "left" }}>
                      &nbsp;
                    </div>
                    <div
                      className="col-lg-11"
                      style={{
                        float: "left",
                        border: "1px solid #eee",
                        padding: "30px",
                        "box-shadow": "0px 0px 10px rgba(0, 0, 0, 0.1)",
                        "border-top-left-radius": "50px",
                        "border-bottom-right-radius": "50px",
                        "border-top-color": "#d82028",
                        "border-bottom-color": "#d82028",
                      }}
                    >
                      <h5
                        style={{
                          "-webkit-text-align": "center",
                          "text-align": "center",
                          "-webkit-text-transform": "uppercase",
                          "text-transform": "uppercase",
                        }}
                      >
                        {author?.authorNav?.name}
                      </h5>
                      <p
                        style={{
                          "-webkit-text-align": "justify",
                          "text-align": "justify",
                          height: "14vh",
                          overflowY: "scroll",
                          padding: "10px 30px 30px 30px",
                        }}
                      >
                        {author?.authorNav?.content}
                      </p>
                      <center>
                        <a
                          // href="#"
                          style={{
                            zoom: "80%",
                            "-webkit-text-decoration": "none",
                            "text-decoration": "none",
                            "background-color": "#d82028",
                            color: "#fff",
                            padding: "10px 20px 10px 20px",
                            "margin-top": "20px",
                            "border-radius": "10px",
                          }}
                          onClick={() => handleAuthor(author)}
                        >
                          View {author?.bookCount}
                        </a>
                      </center>
                    </div>
                  </div>
                );
              }
            })}
        </div>
      </div>
      <Footer />
    </>
  );
}

export default Author;
