import { Provider } from './Provider';
import { User } from './User';

export interface Post {
    id: number;
    images: string[];
    content: string;
    category: string;
    user: User;
    povider: Provider;
    likes: number[];
    createdAt: string;
    updatedAt: string;
}
