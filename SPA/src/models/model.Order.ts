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
interface IUserBalance {
    userBalanceId: number;
    projectId: number;
    userId: number;
    userName: string;
    firstName: string;
    lastName: string;
    avatarUrl: string;
    balance: number;
}

interface IOrderCreditGettingViewModel {
    userId: number;
    projectId: number;
}

interface IOrderCredit {
    orderCreditId: number;
    userId: number;
    projectId: number;
    amount: number;
    creditTime: Date;
    displayedDate: string;
}

interface IUserBalanceUpdateViewModel {
    userBalanceId: number;
    amount: number;
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
    IUserBalance,
    IOrderCredit,
    IUserBalanceUpdateViewModel,
    IOrderCreditGettingViewModel,
    PaymentState,
};
