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

interface IDetailedDataOrder {
    info: IBasicDataOrder;
    products: IBasicDataOrderedProduct[];
}

interface IBasicDataOrder {
    orderId: number;
    userId: number;
    creationDate: Date;
    currentState: State;
}

interface PaymentState {
    state: Payment;
    amount: number;
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


export {
    State,
    Payment,
    IDetailedDataOrder,
    PaymentState,
    IBasicDataOrder,
    IBasicDataOrderedProduct,
};
