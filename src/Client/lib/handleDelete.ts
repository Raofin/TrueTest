import api from '@/lib/api'

export default async function handleDelete(accountId: string): Promise<boolean> {
  if (!accountId) return false

  try {
    const response = await api.delete(`/Account/Delete/${accountId}`)
    if (response.status === 200) {
      return true
    } else {
      console.error('Failed to delete record:', response.status)
      return false
    }
  } catch (error) {
    console.error('Error deleting record:', error)
    return false
  }
}
