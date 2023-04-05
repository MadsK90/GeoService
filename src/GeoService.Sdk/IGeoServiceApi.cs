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

    #region Fibres

    [Post(ApiRoutes.Fibres.CreateFibre)]
    Task<ApiResponse<CreateFibreResponse>> CreateFibreAsync(CreateFibreRequest request);

    [GetWithReplaceId(ApiRoutes.Fibres.GetFibreById)]
    Task<ApiResponse<GetFibreByIdResponse>> GetFibreByIdAsync(GetFibreByIdRequest request);

    [Put(ApiRoutes.Fibres.UpdateFibre)]
    Task<ApiResponse<UpdateFibreResponse>> UpdateFibreAsync(UpdateFibreRequest request);

    [DeleteWithReplaceId(ApiRoutes.Fibres.DeleteFibre)]
    Task<ApiResponse<IResult>> DeleteFibreAsync(DeleteFibreRequest request);

    #endregion Fibres

    #region Manholes

    [Post(ApiRoutes.Manholes.CreateManhole)]
    Task<ApiResponse<CreateManholeResponse>> CreateManholeAsync(CreateManholeRequest request);

    [GetWithReplaceId(ApiRoutes.Manholes.GetManholeById)]
    Task<ApiResponse<GetManholeByIdResponse>> GetManholeByIdAsync(GetManholeByIdRequest request);

    [Put(ApiRoutes.Manholes.UpdateManhole)]
    Task<ApiResponse<UpdateManholeResponse>> UpdateManholeAsync(UpdateManholeRequest request);

    [DeleteWithReplaceId(ApiRoutes.Manholes.DeleteManhole)]
    Task<ApiResponse<IResult>> DeleteManholeAsync(DeleteManholeRequest request);

    #endregion Manholes
}