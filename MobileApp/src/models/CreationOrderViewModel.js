import DataProductToOrder from './DataProductToOrderViewModel.js';

class OrderViewModel {
    constructor(StorageId, UserId, ClassroomId, DataProductToOrder) {      
        this.StorageId = StorageId;
        this.UserId = UserId;
        this.ClassroomId = ClassroomId;
        this.StorageLinkedProductId = DataProductToOrder.StorageLinkedProductId;
        this.Quantity = DataProductToOrder.Quantity;
    }
}
export default OrderViewModel