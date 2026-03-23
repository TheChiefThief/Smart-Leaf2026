import api from "./axiosInstance";

export const profileApi = {
  getProfile: async (userId) => {
    const { data } = await api.get(`/profile/${userId}`);
    return data;
  },

  updateMyProfile: async ({ fullName, avatarUrl, biography, isPrivate }) => {
    await api.put("/profile/me", { fullName, avatarUrl, biography, isPrivate });
  },
};
