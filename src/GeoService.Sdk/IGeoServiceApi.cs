namespace GeoService.Sdk;

public interface IGeoServiceApi
{
    #region Cabinets

    [Post(ApiRoutes.Cabinets.CreateCabinet)]
    Task<ApiResponse<CreateCabinetResponse>> CreateCabinetAsync(CreateCabinetRequest request);

    [GetWithReplaceId(ApiRoutes.Cabinets.GetCabinetById)]
    Task<ApiResponse<GetCabinetByIdResponse>> GetCabinetByIdAsync(GetCabinetByIdRequest request);

    [Put(ApiRoutes.Cabinets.UpdateCabinet)]
    Task<ApiResponse<UpdateCabinetResponse>> UpdateCabinetAsync(UpdateCabinetRequest request);

    [DeleteWithReplaceId(ApiRoutes.Cabinets.DeleteCabinet)]
    Task<ApiResponse<IResult>> DeleteCabinetAsync(DeleteCabinetRequest request);

    #endregion Cabinets

    /*

    #region Fibres

    [Post(ApiRoutes.Fibres.CreateFibre)]
    Task<ApiResponse<CreateFibreResponse>> CreateFibreAsync(CreateFibreRequest request);

    [Get(ApiRoutes.Fibres.GetFibreById)]
    Task<ApiResponse<GetFibreByIdResponse>> GetFibreByIdAsync(GetFibreByIdRequest request);

    [Put(ApiRoutes.Fibres.UpdateFibre)]
    Task<ApiResponse<UpdateFibreResponse>> UpdateFibreAsync(UpdateFibreRequest request);

    [Delete(ApiRoutes.Fibres.DeleteFibre)]
    Task<IResult> DeleteFibreResponse(DeleteFibreRequest request);

    #endregion Fibres

    */
}