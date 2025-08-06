import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ROUTES } from "./routes";

import LoginPage from "../../pages/account/LoginPage";
import RegisterPage from "../../pages/account/RegisterPage";

import HomePage from "../../pages/HomePage";
import AboutPage from "../../pages/contactAndAbout/AboutPage";
import ManageAboutUs from "../../pages/management/ManageAboutUs";
import ContactPage from "../../pages/contactAndAbout/ContactPage";
import ManageContactUs from "../../pages/management/ManageContactUs";
import NotFoundPage from "../../pages/errors/NotFoundPage";

import AccountPage from "../../pages/account/AccountPage";

import CarMeetHomePage from "../../pages/carMeet/CarMeetHomePage";
import CreateCarMeetPage from "../../pages/carMeet/CreateCarMeetPage";
import CarMeetDetailsPage from "../../pages/carMeet/CarMeetDetailsPage";
import CarMeetEditPage from "../../pages/carMeet/CarMeetEditPage";

import CarForumHomePage from "../../pages/carForum/CarForumHomePage";
import CreateCarForumPage from "../../pages/carForum/CreateCarForumPage";
import CarForumDetailsPage from "../../pages/carForum/CarForumDetailsPage";
import CarForumEditPage from "../../pages/carForum/CarForumEditPage";

import CarServiceHomePage from "../../pages/carService/CarServiceHomePage";
import CreateCarServicePage from "../../pages/carService/CreateCarServicePage";
import CarServiceDetailsPage from "../../pages/carService/CarServiceDetailsPage";
import CarServiceEditPage from "../../pages/carService/CarServiceEditPage";

import CarShopHomePage from "../../pages/carShop/CarShopHomePage";
import CreateCarShopPage from "../../pages/carShop/CreateCarShopPage";
import CarShopDetailsPage from "../../pages/carShop/CarShopDetailsPage";
import CarShopEditPage from "../../pages/carShop/CarShopEditPage";

import ManagementPage from "../../pages/management/ManagementPage";
import ManageCarServiceCategories from "../../pages/management/ManageCarServiceCategories";
import ManageCarForumBrands from "../../pages/management/ManageCarForumBrands";
import ManageCarShopBrands from "../../pages/management/ManageCarShopBrands";

import ComingSoonPage from "../../pages/errors/ComingSoonPage";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path={ROUTES.home} element={<HomePage />} />
        <Route path={ROUTES.login} element={<LoginPage />} />
        <Route path={ROUTES.register} element={<RegisterPage />} />
        <Route path={ROUTES.profile} element={<AccountPage />} />
        <Route path={ROUTES.settings} element={<AccountPage />} />

        <Route path={ROUTES.about} element={<AboutPage />} />
        <Route path={ROUTES.aboutEdit} element={<ManageAboutUs />} />
        <Route path={ROUTES.contact} element={<ContactPage />} />
        <Route path={ROUTES.contactEdit} element={<ManageContactUs />} />

        <Route path={ROUTES.carMeet} element={<CarMeetHomePage />} />
        <Route path={ROUTES.carMeetCreate} element={<CreateCarMeetPage />} />
        <Route path={ROUTES.carMeetDetails} element={<CarMeetDetailsPage />} />
        <Route path={ROUTES.carMeetEdit} element={<CarMeetEditPage />} />

        <Route path={ROUTES.carForum} element={<CarForumHomePage />} />
        <Route path={ROUTES.carForumCreate} element={<CreateCarForumPage />} />
        <Route path={ROUTES.carForumDetails} element={<CarForumDetailsPage />} />
        <Route path={ROUTES.carForumEdit} element={<CarForumEditPage />} />

        <Route path={ROUTES.carService} element={<CarServiceHomePage />} />
        <Route path={ROUTES.carServiceCreate} element={<CreateCarServicePage />} />
        <Route path={ROUTES.carServiceDetails} element={<CarServiceDetailsPage />} />
        <Route path={ROUTES.carServiceEdit} element={<CarServiceEditPage />} />

        <Route path={ROUTES.carShop} element={<CarShopHomePage />} />
        <Route path={ROUTES.carShopCreate} element={<CreateCarShopPage />} />
        <Route path={ROUTES.carShopDetails} element={<CarShopDetailsPage />} />
        <Route path={ROUTES.carShopEdit} element={<CarShopEditPage />} />

        <Route path={ROUTES.management} element={<ManagementPage />} />

        <Route path="/management/service-categories" element={<ManageCarServiceCategories />} />
        <Route path="/management/forum-brands" element={<ManageCarForumBrands />} />
        <Route path="/management/shop-brands" element={<ManageCarShopBrands />} />

        <Route path="/coming-soon" element={<ComingSoonPage />} />

        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  );
}
