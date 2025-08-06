import { useSearchParams } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import CarHeader from "../../components/shared/CarHeader"
import Footer from "../../components/shared/Footer";
import Pagination from "../../components/shared/Pagination";
import useCarServiceListings from "../../hooks/useCarServiceListings";

export default function CarServiceHomePage() {
  const { isAuthenticated } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();
  const { services, totalPages, loading, error, page } = useCarServiceListings(searchParams, isAuthenticated);

  const updateParams = (params) => {
    const newParams = new URLSearchParams(searchParams);
    Object.entries(params).forEach(([key, val]) => {
      if (val === null || val === undefined || val === "") {
        newParams.delete(key);
      } else {
        newParams.set(key, val);
      }
    });
    setSearchParams(newParams);
  };

  const handlePageChange = (newPage) => updateParams({ Page: newPage.toString() });

  const handleFilterModeChange = (mode) => {
    updateParams({
      SavedOnly: mode === "saved" ? "true" : null,
      MyServicesOnly: mode === "mine" ? "true" : null,
      Page: "1",
    });
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader
        searchTerm={searchParams.get("SearchTerm") || ""}
        setSearchTerm={(term) => updateParams({ SearchTerm: term, Page: "1" })}
      />

      <div className="flex flex-1 p-4">
        <div className="w-64 mr-4">
          <FilterSidebar
            filterMode={searchParams.get("SavedOnly") === "true" ? "saved" : searchParams.get("MyServicesOnly") === "true" ? "mine" : "all"}
            setFilterMode={handleFilterModeChange}
            minPrice={searchParams.get("MinPrice")}
            maxPrice={searchParams.get("MaxPrice")}
            setMinPrice={(val) => updateParams({ MinPrice: val })}
            setMaxPrice={(val) => updateParams({ MaxPrice: val })}
          />
        </div>

        <div className="flex-1 flex flex-col justify-between">
          {loading ? (
            <div className="text-center text-gray-600 w-full py-20">Loading...</div>
          ) : error ? (
            <div className="text-center text-red-500 w-full py-20">{error}</div>
          ) : services.length === 0 ? (
            <div className="text-center text-gray-500 w-full py-20">No car services found.</div>
          ) : (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mb-4">
                {services.map((service) => (
                  <ListingCard key={service.id} service={service} />
                ))}
              </div>
              <div className="mt-auto">
                <Pagination
                  currentPage={page}
                  totalPages={totalPages}
                  onPageChange={handlePageChange}
                />
              </div>
            </>
          )}
        </div>
      </div>

      <Footer />
    </div>
  );
}

import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { saveCarServiceListing, unsaveCarServiceListing } from "../../services/api/carServiceListingService";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import ActiveSaveIcon from "../../assets/CarSpaceActiveSaveIconLightMode.png";

function ListingCard({ service }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [isSaved, setIsSaved] = useState(service.isSavedByCurrentUser);
  const imageUrl = service.imageUrl
    ? `${import.meta.env.VITE_API_BASE_URL}${service.imageUrl}`
    : "https://via.placeholder.com/300x200";

  const toggleSave = async (e) => {
    e.preventDefault();
    if (!isAuthenticated) return navigate("/login");

    try {
      isSaved
        ? await unsaveCarServiceListing(service.id)
        : await saveCarServiceListing(service.id);
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Save toggle failed:", err);
    }
  };

  return (
    <Link
      to={`/car-service/${service.id}`}
      className="bg-white border border-gray-300 rounded-lg hover:shadow-md transition p-3 flex flex-col justify-between h-[350px] relative"
    >
      <div className="flex justify-between items-center text-xs text-gray-600 font-medium">
        <span>{service.userNickname || "Unknown"}</span>
        <img
          src={isSaved ? ActiveSaveIcon : SaveIcon}
          alt="Save"
          onClick={toggleSave}
          className="w-8 h-8 cursor-pointer"
          title={isSaved ? "Unsave" : "Save"}
        />
      </div>
      <div className="text-xs text-gray-500 mb-2">
        Updated: {new Date(service.updatedAt).toLocaleDateString()}
      </div>
      <div className="w-full h-36 bg-gray-200 rounded-md overflow-hidden mb-2">
        <img src={imageUrl} alt={service.title} className="w-full h-full object-cover" />
      </div>
      <div className="text-sm text-center mb-2">
        <h3 className="font-semibold">{service.title}</h3>
        <p className="text-gray-600 truncate">{service.description}</p>
        <p className="text-blue-600 font-semibold mt-1">
          {service.price ? `${service.price} BGN` : "Price not set"}
        </p>
      </div>
      <div className="flex justify-between text-xs text-gray-500 mt-auto">
        <span>{service.city}</span>
        <span>{service.categoryName}</span>
      </div>
    </Link>
  );
}

import { ROUTES } from "../../utils/routes/routes";

function FilterSidebar({ filterMode, setFilterMode, minPrice, maxPrice, setMinPrice, setMaxPrice }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const tab = (label, active, onClick) => (
    <div
      onClick={onClick}
      className={`text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 transition ${
        active ? "bg-blue-600 text-white" : "bg-white text-black hover:bg-blue-100 cursor-pointer"
      }`}
    >
      {label}
    </div>
  );

  const handleCreate = () => {
    navigate(isAuthenticated ? ROUTES.carServiceCreate : ROUTES.login);
  };

  const handleProtected = (mode) => {
    isAuthenticated ? setFilterMode(mode) : navigate(ROUTES.login);
  };

  const handleClear = () => {
    setMinPrice(null);
    setMaxPrice(null);
    navigate(window.location.pathname, { replace: true });
  };

  return (
    <aside className="w-full h-full pr-4 border-r border-gray-300 text-sm space-y-4">
      <button
        onClick={handleCreate}
        className="w-full text-center bg-white border border-blue-600 text-blue-600 rounded-md py-2 font-semibold hover:bg-blue-600 hover:text-white transition"
      >
        + Create Service
      </button>

      <div className="space-y-1">
        {tab("All Services", filterMode === "all", () => setFilterMode("all"))}
        {tab("My Services", filterMode === "mine", () => handleProtected("mine"))}
        {tab("Saved Services", filterMode === "saved", () => handleProtected("saved"))}
      </div>

      <div className="pt-4 space-y-2">
        <div onClick={handleClear} className={tab("Clear All Filters", false, handleClear).props.className}>
          Clear All Filters
        </div>

        <InputWithClear
          placeholder="Min Price"
          value={minPrice}
          setValue={setMinPrice}
        />

        <InputWithClear
          placeholder="Max Price"
          value={maxPrice}
          setValue={setMaxPrice}
        />
      </div>
    </aside>
  );
}

function InputWithClear({ value, setValue, placeholder }) {
  return (
    <div className="relative">
      <input
        type="text"
        inputMode="decimal"
        placeholder={placeholder}
        value={value ?? ""}
        onChange={(e) => setValue(e.target.value === "" ? null : parseFloat(e.target.value))}
        className="w-full border bg-white border-gray-300 rounded-md px-3 py-1 pr-8 focus:outline-none focus:ring focus:ring-blue-200 appearance-none"
      />
      {value !== null && (
        <button
          onClick={() => setValue(null)}
          type="button"
          className="absolute right-2 top-1/2 -translate-y-1/2 text-gray-500 hover:text-red-500 text-sm cursor-pointer"
          title="Clear"
        >
          ✕
        </button>
      )}
    </div>
  );
}
