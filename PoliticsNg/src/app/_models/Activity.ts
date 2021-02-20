import { User } from './User';

export interface ActivityLike{
    likes: number[];
    dislikes: number[];
}

export interface ActivityComment {
    id: number,
    content: string,
    side: string,
    likes: number[],
    user: User,
}
export interface Activity {
    id: number;
    title: string;
    description: string;
    comments: ActivityComment[];
    likes: ActivityLike;
}
