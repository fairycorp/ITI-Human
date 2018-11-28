// --------------------------------------

// Enumerators.

enum State {
    NotStarted, Underway, Paused, Delivered, Canceled
}

enum Payment {
    Unpaid, Paid, Credited
}

// --------------------------------------

// Interfaces.

interface IBasicDataOrder {
    orderId: number;
    userId: number;
    creationDate: Date;
    currentState: State;
}

interface IBasicDataOrderedProduct {
    orderedProductId: number;
    orderId: number;
    storageLinkedProductId: number;
    name: string;
    desc: string;
    unitPrice: number;
    quantity: number;
    currentState: State;
    payment: PaymentState;

}

interface ICreationViewModel {
    storageId: number;
    userId: number;
    classroomId: number;
    products: IBasicDataOrderedProduct[];
}

interface IDetailedDataOrder {
    info: IBasicDataOrder;
    products: IBasicDataOrderedProduct[];
}

interface PaymentState {
    state: Payment;
    amount: number;
}

export {
    State,
    Payment,
    IBasicDataOrder,
    IBasicDataOrderedProduct,
    ICreationViewModel,
    IDetailedDataOrder,
    PaymentState,
};
