import { Post } from "./Post";

export interface User {
    id: number;
    userName: string;
    firstname: string;
    lastname: string;
    geoId: string;
    phone: string;
    email: string;
    avatar: string;
    posts: Post[];
    createdAt: string;
    updatedAt: string;
}
