interface IDetailedDataOrder {
    orderInfo: IBasicDataOrder;
    products: IBasicDataOrderedProduct[];
}

interface IBasicDataOrder {
    orderId: number;
    userId: number;
    creationDate: Date;
    hasBeenEntirelyDelivered: boolean;
}

interface IBasicDataOrderedProduct {
    orderedProductId: number;
    orderId: number;
    productId: number;
    name: string;
    desc: string;
    hasBeenDelivered: boolean;
}
export { IDetailedDataOrder, IBasicDataOrder, IBasicDataOrderedProduct };
