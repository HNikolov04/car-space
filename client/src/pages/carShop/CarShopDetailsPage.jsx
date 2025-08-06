import { useParams, useNavigate, Link } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import SaveIconActive from "../../assets/CarSpaceActiveSaveIconLightMode.png";
import useCarShopDetails from "../../hooks/useCarShopDetails";

export default function CarShopDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();
  const {
    car,
    isSaved,
    loading,
    toggleSave,
    handleDelete,
  } = useCarShopDetails(id, navigate);

  if (loading || !car) {
    return <div className="text-center py-20">Loading...</div>;
  }

  const imageUrl = car.imageUrl
    ? `${import.meta.env.VITE_API_BASE_URL}${car.imageUrl}`
    : "https://via.placeholder.com/600x300";

  const isOwner = user?.id === car.userId;

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8">
          <div className="flex justify-between items-start mb-4">
            <div>
              <div className="text-sm text-gray-700 font-medium">
                {car.userNickname || "Unknown"}
              </div>
              <div className="text-xs text-gray-500">
                Updated: {new Date(car.updatedAt).toLocaleDateString()}
              </div>
            </div>
            <img
              src={isSaved ? SaveIconActive : SaveIcon}
              alt={isSaved ? "Saved" : "Save"}
              className="w-10 h-10 cursor-pointer transition-transform hover:scale-110"
              onClick={toggleSave}
              title={isSaved ? "Unsave" : "Save"}
            />
          </div>

          <div className="w-full h-64 bg-gray-200 rounded-md overflow-hidden mb-4">
            <img src={imageUrl} alt={car.title} className="w-full h-full object-cover" />
          </div>

          <div className="text-lg font-semibold text-blue-700 mb-2">€{car.price}</div>
          <h2 className="text-2xl font-bold text-gray-800 mb-2">
            {car.title} {car.engine}
          </h2>
          <p className="text-sm text-gray-600 mb-4">
            {car.year} • {car.fuelType} • {car.mileage.toLocaleString()} km
          </p>
          <p className="text-gray-700 mb-6">{car.description}</p>

          <div className="grid grid-cols-2 gap-4 text-sm text-gray-700 mb-6">
            <div><strong>Brand:</strong> {car.brandName}</div>
            <div><strong>Model:</strong> {car.model}</div>
            <div><strong>Horsepower:</strong> {car.horsepower} HP</div>
            <div><strong>Transmission:</strong> {car.transmission}</div>
            <div><strong>Fuel Type:</strong> {car.fuelType}</div>
            <div><strong>Color:</strong> {car.color}</div>
            <div><strong>Euro Standard:</strong> {car.euroStandard}</div>
            <div><strong>Doors:</strong> {car.doors}</div>
            <div><strong>Location:</strong> {car.city}, {car.address}</div>
          </div>

          <div className="mt-8 flex justify-center gap-4 flex-wrap items-center">
            {isOwner && (
              <Link
                to={`/car-shop/edit/${car.id}`}
                className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                Edit
              </Link>
            )}

            <Link
              to="/car-shop"
              className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Back to Listings
            </Link>

            {isOwner && (
              <button
                onClick={handleDelete}
                className="px-4 py-2 cursor-pointer bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
              >
                Delete
              </button>
            )}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
