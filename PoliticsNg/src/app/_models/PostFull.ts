import { Provider } from './Provider';
import { User } from './User';
import { Image } from './Image';
import { Category } from './Category';

export interface PostFull {
    id: number;
    images: Image[];
    content: string;
    category: Category;
    user: User;
    povider: Provider;
    likes: number[];
    createdAt: string;
    updatedAt: string;
}
