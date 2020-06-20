export interface UserProfile {
  userId: number,
  name: string,
  surname: string,
  middlename?: string,
  contactEmail: string,
  dateBirth?: Date,
  mainPhotoId?: number
}
