package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.model.auth.LoginUserInfo;
import foundation.softwaredesign.orbi.model.auth.RegisterUserInfo;
import foundation.softwaredesign.orbi.service.UserService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.Response;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/auth")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class AuthorizationRestApi {

    @Inject
    UserService userService;

    @GET
    @Path("/user")
    public Response user() {
        // will fail if not authorized - filter
        return Response.status(Response.Status.OK).build();
    }

    @POST
    @Path("/login")
    public AuthorizationInfo login(@NotNull LoginUserInfo loginUserInfo) {
        return userService.login(loginUserInfo);
    }

    @POST
    @Path("/requestpassword")
    public Response register(@NotNull RegisterUserInfo registerUserInfo) {
        userService.requestPassword(registerUserInfo);
        return Response.status(Response.Status.CREATED).build();
    }


}
