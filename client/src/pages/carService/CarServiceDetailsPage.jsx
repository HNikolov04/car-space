import { useParams, useNavigate, Link } from "react-router-dom";
import Footer from "../../components/shared/Footer";
import CarHeader from "../../components/shared/CarHeader";
import useCarServiceDetails from "../../hooks/useCarServiceDetails";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import ActiveSaveIcon from "../../assets/CarSpaceActiveSaveIconLightMode.png";

export default function CarServiceDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const {
    service,
    isOwner,
    isSaved,
    toggleSave,
    handleDelete,
    loading,
    error
  } = useCarServiceDetails(id, navigate);

  if (loading) return <div className="text-center py-20">Loading...</div>;
  if (error) return <div className="text-center text-red-500 py-20">{error}</div>;
  if (!service) return null;

  const imageUrl = service.imageUrl
    ? `${import.meta.env.VITE_API_BASE_URL}${service.imageUrl}`
    : "https://via.placeholder.com/600x300";

  const baseBtn = "px-4 py-2 cursor-pointer rounded-md shadow text-white font-semibold";
  const btnBlue = `${baseBtn} bg-blue-600 hover:bg-blue-700`;
  const btnRed = `${baseBtn} bg-red-500 hover:bg-red-600`;

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <CarHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8">

          <div className="flex justify-between items-start mb-4">
            <div>
              <div className="text-sm text-gray-700 font-medium">
                {service.userNickname || "Unknown"}
              </div>
              <div className="text-xs text-gray-500">
                Updated: {new Date(service.updatedAt).toLocaleDateString()}
              </div>
            </div>
            <img
              src={isSaved ? ActiveSaveIcon : SaveIcon}
              alt="Save"
              onClick={toggleSave}
              className="w-10 h-10 cursor-pointer transition-transform hover:scale-110"
              title={isSaved ? "Unsave" : "Save"}
            />
          </div>

          <div className="w-full h-64 bg-gray-200 rounded-md overflow-hidden mb-4">
            <img
              src={imageUrl}
              alt={service.title}
              className="w-full h-full object-cover"
            />
          </div>

          <h1 className="text-2xl font-bold mb-2">{service.title}</h1>
          <p className="text-gray-700 mb-4">{service.description}</p>

          <div className="text-lg font-semibold text-blue-700 mb-4">
            Price: {service.price ? `${service.price} BGN` : "Free"}
          </div>

          <div className="text-sm text-gray-600 mb-6">
            {service.city}, {service.address}
          </div>

          <div className="flex justify-between text-sm text-gray-600">
            <span>📞 {service.phoneNumber}</span>
            <span>Category: {service.categoryName}</span>
          </div>

          <div className="mt-8 flex justify-center gap-4 flex-wrap items-center">
            {isOwner ? (
              <>
                <Link to={`/car-service/edit/${service.id}`} className={btnBlue}>
                  Edit
                </Link>
                <Link to="/car-service" className={btnBlue}>
                  Back to Services
                </Link>
                <button onClick={handleDelete} className={btnRed}>
                  Delete
                </button>
              </>
            ) : (
              <Link to="/car-service" className={btnBlue}>
                Back to Services
              </Link>
            )}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
