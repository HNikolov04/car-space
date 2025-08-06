import { useSearchParams } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import Pagination from "../../components/shared/Pagination";
import useCarShopListings from "../../hooks/useCarShopListings";

export default function CarShopHomePage() {
  const { isAuthenticated } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();
  const {
    cars,
    totalPages,
    loading,
    error,
    searchTerm,
    updateParams,
    handlePageChange,
    handleFilterModeChange,
    filterParams,
  } = useCarShopListings(searchParams, isAuthenticated);

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader
        searchTerm={searchTerm}
        setSearchTerm={(val) => updateParams({ SearchTerm: val, Page: "1" })}
      />

      <div className="flex flex-1 p-4">
        <div className="w-64 mr-4">
          <FilterSidebar
            filterMode={
              filterParams.savedOnly
                ? "saved"
                : filterParams.myListingsOnly
                ? "mine"
                : "all"
            }
            setFilterMode={handleFilterModeChange}
            brandId={filterParams.brandId}
            setBrandId={(val) => updateParams({ BrandId: val })}
            minPrice={filterParams.minPrice}
            maxPrice={filterParams.maxPrice}
            setMinPrice={(val) => updateParams({ MinPrice: val })}
            setMaxPrice={(val) => updateParams({ MaxPrice: val })}
          />
        </div>

        <div className="flex-1 flex flex-col justify-between">
          {loading ? (
            <div className="text-center text-gray-600 py-20">Loading...</div>
          ) : error ? (
            <div className="text-center text-red-500 py-20">{error}</div>
          ) : cars.length === 0 ? (
            <div className="text-center text-gray-500 py-20">No listings found.</div>
          ) : (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {cars.map((car) => (
                  <ListingCard key={car.id} car={car} />
                ))}
              </div>
              <div className="mt-auto pt-6">
                <Pagination
                  currentPage={parseInt(searchParams.get("Page") || "1")}
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
import { saveCarShopListing, unsaveCarShopListing } from "../../services/api/carShopListingService";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import ActiveSaveIcon from "../../assets/CarSpaceActiveSaveIconLightMode.png";

function ListingCard({ car }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [isSaved, setIsSaved] = useState(car.isSavedByCurrentUser || false);

  const imageUrl = car.imageUrl
    ? `${import.meta.env.VITE_API_BASE_URL}${car.imageUrl}`
    : "https://via.placeholder.com/300x200";

  const handleSaveToggle = async (e) => {
    e.preventDefault();

    if (!isAuthenticated) {
      navigate("/login");
      return;
    }

    try {
      if (isSaved) {
        await unsaveCarShopListing(car.id);
      } else {
        await saveCarShopListing(car.id);
      }
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Save toggle failed:", err);
    }
  };

  return (
    <Link
      to={`/car-shop/${car.id}`}
      className="bg-white border border-gray-300 rounded-lg hover:shadow-md transition p-3 flex flex-col justify-between h-[400px] relative"
    >
      <div className="flex justify-between items-center text-xs text-gray-600 font-medium">
        <span>{car.userNickname || "Unknown"}</span>
        <img
          src={isSaved ? ActiveSaveIcon : SaveIcon}
          alt="Save"
          onClick={handleSaveToggle}
          className="w-8 h-8 cursor-pointer"
          title={isSaved ? "Unsave" : "Save"}
        />
      </div>

      <div className="text-xs text-gray-500 mb-2">
        Updated: {new Date(car.updatedAt).toLocaleDateString()}
      </div>

      <div className="w-full h-36 bg-gray-200 rounded-md overflow-hidden mb-2">
        <img src={imageUrl} alt={car.title} className="w-full h-full object-cover" />
      </div>

      <div className="text-sm text-center mb-2">
        <h3 className="font-semibold text-base">{car.title}</h3>
        <p className="text-gray-500 text-xs mb-1">
          {car.year} • {car.fuelType} • {car.mileage.toLocaleString()} km
        </p>
        <p className="text-gray-600 truncate">{car.description}</p>
        <p className="text-blue-600 font-semibold mt-1">€{car.price}</p>
      </div>

      <div className="flex justify-between text-xs text-gray-500 mt-auto">
        <span>{car.city}</span>
        <span>{car.brandName}, {car.model}</span>
      </div>
    </Link>
  );
}

import { useEffect } from "react";
import { ROUTES } from "../../utils/routes/routes";
import { getCarShopBrands } from "../../services/api/carShopBrandService";

function FilterSidebar({
  filterMode,
  setFilterMode,
  brandId,
  setBrandId,
  minPrice,
  maxPrice,
  setMinPrice,
  setMaxPrice,
}) {
  const [brands, setBrands] = useState([]);
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    getCarShopBrands()
      .then(setBrands)
      .catch((err) => console.error("Failed to load brands:", err));
  }, []);

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
    navigate(isAuthenticated ? ROUTES.carShopCreate : ROUTES.login);
  };

  const handleProtected = (mode) => {
    isAuthenticated ? setFilterMode(mode) : navigate(ROUTES.login);
  };

  const handleClear = () => {
    setBrandId(null);
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
        + Create Listing
      </button>

      <div className="space-y-1">
        {tab("All Listings", filterMode === "all", () => setFilterMode("all"))}
        {tab("My Listings", filterMode === "mine", () => handleProtected("mine"))}
        {tab("Saved Listings", filterMode === "saved", () => handleProtected("saved"))}
      </div>

      <div className="pt-4 space-y-2">
        <div onClick={handleClear} className={tab("Clear All Filters", false, handleClear).props.className}>
          Clear All Filters
        </div>

        <div className="relative">
          <select
            value={brandId ?? ""}
            onChange={(e) => setBrandId(e.target.value === "" ? null : parseInt(e.target.value))}
            className="w-full bg-white border border-gray-300 rounded-md px-3 py-1 pr-8 text-sm cursor-pointer appearance-none"
          >
            <option value="">All Brands</option>
            {brands.map((brand) => (
              <option key={brand.id} value={brand.id}>
                {brand.name}
              </option>
            ))}
          </select>
          <div className="pointer-events-none absolute right-2 top-1/2 transform -translate-y-1/2 text-gray-600">
            ▼
          </div>
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
