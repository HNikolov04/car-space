import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { ROUTES } from "../../utils/routes/routes";
import { useCarForumHome } from "../../hooks/useCarForumHome";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import Pagination from "../../components/shared/Pagination";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import ActiveSaveIcon from "../../assets/CarSpaceActiveSaveIconLightMode.png";
import {
  saveCarForumArticle,
  unsaveCarForumArticle,
} from "../../services/api/carForumArticleService";

function FilterSidebar({ filterMode, setFilterMode, brandId, setBrandId }) {
  const [brands, setBrands] = useState([
    { id: 1, name: "BMW" },
    { id: 2, name: "Audi" },
  ]); 

  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const handleProtectedClick = (mode) => {
    if (!isAuthenticated) {
      navigate(ROUTES.login);
    } else {
      setFilterMode(mode);
    }
  };

  return (
    <aside className="w-full h-full pr-4 border-r border-gray-300 text-sm space-y-4">
      <button
        onClick={() => navigate(isAuthenticated ? ROUTES.carForumCreate : ROUTES.login)}
        className="w-full text-center cursor-pointer bg-white border border-blue-600 text-blue-600 rounded-md py-2 font-semibold hover:bg-blue-600 hover:text-white transition"
      >
        + Create Article
      </button>

      <div className="space-y-1">
        <div
          className={`text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 transition ${
            filterMode === "all"
              ? "bg-blue-600 text-white"
              : "bg-white text-black hover:bg-blue-100 cursor-pointer"
          }`}
          onClick={() => setFilterMode("all")}
        >
          All Articles
        </div>
        <div
          className={`text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 transition ${
            filterMode === "mine"
              ? "bg-blue-600 text-white"
              : "bg-white text-black hover:bg-blue-100 cursor-pointer"
          }`}
          onClick={() => handleProtectedClick("mine")}
        >
          My Articles
        </div>
        <div
          className={`text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 transition ${
            filterMode === "saved"
              ? "bg-blue-600 text-white"
              : "bg-white text-black hover:bg-blue-100 cursor-pointer"
          }`}
          onClick={() => handleProtectedClick("saved")}
        >
          Saved Articles
        </div>
      </div>

      <div className="pt-4 space-y-2">
        <div
          onClick={() => setBrandId(null)}
          className="text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 bg-white text-black hover:bg-blue-100 cursor-pointer"
        >
          Clear All Filters
        </div>

        <select
          value={brandId ?? ""}
          onChange={(e) =>
            setBrandId(e.target.value === "" ? null : parseInt(e.target.value))
          }
          className="w-full bg-white border border-gray-300 rounded-md px-3 py-1 pr-8 text-sm cursor-pointer appearance-none"
        >
          <option value="">All Brands</option>
          {brands.map((brand) => (
            <option key={brand.id} value={brand.id}>
              {brand.name}
            </option>
          ))}
        </select>
      </div>
    </aside>
  );
}

function CarForumListing({ post }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [isSaved, setIsSaved] = useState(post.isSavedByCurrentUser || false);

  const handleSaveToggle = async (e) => {
    e.preventDefault();

    if (!isAuthenticated) {
      navigate(ROUTES.login);
      return;
    }

    try {
      if (isSaved) {
        await unsaveCarForumArticle(post.id);
      } else {
        await saveCarForumArticle(post.id);
      }
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Failed to toggle save:", err);
    }
  };

  return (
    <Link
      to={`/car-forum/${post.id}`}
      className="bg-white border border-gray-300 rounded-lg hover:shadow-md transition p-4 flex flex-col justify-between space-y-2"
    >
      <div className="flex justify-between items-center text-xs text-gray-600 font-medium">
        <span>{post.userNickname || "Unknown"}</span>
        <img
          src={isSaved ? ActiveSaveIcon : SaveIcon}
          alt="Save"
          onClick={handleSaveToggle}
          className="w-8 h-8 cursor-pointer"
          title={isSaved ? "Unsave" : "Save"}
        />
      </div>
      <div className="text-xs text-gray-500 mb-2">
        Updated: {new Date(post.updatedAt).toLocaleDateString()}
      </div>

      <h3 className="text-lg font-semibold">{post.title}</h3>

      <p className="text-gray-700 text-sm">
        {post.description.length > 30
          ? post.description.split(" ").slice(0, 15).join(" ") + "..."
          : post.description}
      </p>

      <div className="flex justify-between items-center text-xs text-gray-500 mt-auto">
        {post.brandName && <span className="italic">Brand: {post.brandName}</span>}
        <span />
      </div>
    </Link>
  );
}

export default function CarForumHomePage() {
  const {
    posts,
    totalPages,
    page,
    searchTerm,
    brandId,
    filterMode,
    setSearchTerm,
    setBrandId,
    handleFilterModeChange,
    handlePageChange,
    loading,
    error,
  } = useCarForumHome();

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader
        searchTerm={searchTerm}
        setSearchTerm={(term) => setSearchTerm(term)}
      />

      <div className="flex flex-1 p-4">
        <div className="w-64 mr-4">
          <FilterSidebar
            filterMode={filterMode}
            setFilterMode={handleFilterModeChange}
            brandId={brandId}
            setBrandId={setBrandId}
          />
        </div>

        <div className="flex-1 flex flex-col justify-between">
          {loading ? (
            <div className="text-center py-20 text-gray-500">Loading articles...</div>
          ) : error ? (
            <div className="text-center py-20 text-red-500">{error}</div>
          ) : posts.length === 0 ? (
            <div className="text-center py-20 text-gray-500">No articles found.</div>
          ) : (
            <>
              <div className="space-y-4 mb-4">
                {posts.map((post) => (
                  <CarForumListing key={post.id} post={post} />
                ))}
              </div>

              <Pagination
                currentPage={page}
                totalPages={totalPages}
                onPageChange={handlePageChange}
              />
            </>
          )}
        </div>
      </div>

      <Footer />
    </div>
  );
}
