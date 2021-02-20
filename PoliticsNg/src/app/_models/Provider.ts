import { User } from './User';
import { Post } from './Post';

export interface Provider {
    id: number;
    name: string;
    title: string;
    image: string;
    moderator: number;
    members: User[];
    posts: Post[];
    createdAt: string;
    updatedAt: string;
}
