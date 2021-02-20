import { User } from './User';

export interface TopicLikes {
    userId: number[],
    sum: number
}

export interface TopicComment {
    id: number,
    content: string,
    side: string,
    likes: number[],
    user: User,
}

export interface RatingTopicLite {
    id: number,
    title: string,
    description: string,
    comments: number,
    likes: TopicLikes,
}

export interface RatingTopic {
    id: number,
    title: string,
    description: string,
    comments: TopicComment[],
    likes: TopicLikes,
}