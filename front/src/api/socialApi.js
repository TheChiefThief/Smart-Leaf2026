import api from "./axiosInstance";

export const socialApi = {
  getFeed: async () => {
    const { data } = await api.get("/social/feed");
    return data;
  },

  createPost: async ({ plantId, topicId, imageUrl, descripcion }) => {
    const { data } = await api.post("/social/post", { plantId, topicId, imageUrl, descripcion });
    return data;
  },

  likePost: async (postId) => {
    await api.post(`/social/like/${postId}`);
  },

  unlikePost: async (postId) => {
    await api.delete(`/social/like/${postId}`);
  },

  commentPost: async (postId, content) => {
    await api.post(`/social/comment/${postId}`, { content });
  },

  followUser: async (targetUserId) => {
    await api.post(`/social/follow/${targetUserId}`);
  },

  unfollowUser: async (targetUserId) => {
    await api.delete(`/social/follow/${targetUserId}`);
  },

  askChatbot: async (history) => {
    const { data } = await api.post("/chatbot/ask", history);
    return data;
  },
};
